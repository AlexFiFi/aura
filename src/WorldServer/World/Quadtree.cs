// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Aura.World.World
{
	public interface IQuadtreeObject
	{
		Rectangle Rect { get; }
	}

	public class Quadtree<TObject> where TObject : IQuadtreeObject
	{
		private const int MaxObjects = 2;

		public Rectangle Rect { get; protected set; }

		public Quadtree<TObject> TopLeftChild { get; protected set; }
		public Quadtree<TObject> TopRightChild { get; protected set; }
		public Quadtree<TObject> BottomLeftChild { get; protected set; }
		public Quadtree<TObject> BottomRightChild { get; protected set; }

		public List<TObject> Objects { get; protected set; }

		public Quadtree(Rectangle rect)
		{
			this.Rect = rect;
		}

		public Quadtree(int x, int y, int width, int height)
			: this(new Rectangle(x, y, width, height))
		{ }

		private void Add(TObject item)
		{
			if (this.Objects == null)
				this.Objects = new List<TObject>();

			this.Objects.Add(item);
		}

		private void Remove(TObject item)
		{
			if (this.Objects != null && this.Objects.Contains(item))
				this.Objects.Remove(item);
		}

		private void Split()
		{
			var size = new Point(this.Rect.Width / 2, this.Rect.Height / 2);
			var mid = new Point(this.Rect.X + size.X, this.Rect.Y + size.Y);

			this.TopLeftChild = new Quadtree<TObject>(new Rectangle(this.Rect.Left, this.Rect.Top, size.X, size.Y));
			this.TopRightChild = new Quadtree<TObject>(new Rectangle(mid.X, this.Rect.Top, size.X, size.Y));
			this.BottomLeftChild = new Quadtree<TObject>(new Rectangle(this.Rect.Left, mid.Y, size.X, size.Y));
			this.BottomRightChild = new Quadtree<TObject>(new Rectangle(mid.X, mid.Y, size.X, size.Y));

			for (int i = 0; i < this.Objects.Count; i++)
			{
				var destTree = this.GetDestinationTree(this.Objects[i]);

				if (destTree != this)
				{
					destTree.Insert(this.Objects[i]);
					this.Remove(this.Objects[i]);
					i--;
				}
			}
		}

		private Quadtree<TObject> GetDestinationTree(TObject item)
		{
			var destTree = this;

			if (this.TopLeftChild.Rect.Contains(item.Rect))
				destTree = this.TopLeftChild;
			else if (this.TopRightChild.Rect.Contains(item.Rect))
				destTree = this.TopRightChild;
			else if (this.BottomLeftChild.Rect.Contains(item.Rect))
				destTree = this.BottomLeftChild;
			else if (this.BottomRightChild.Rect.Contains(item.Rect))
				destTree = this.BottomRightChild;

			return destTree;
		}

		public void Clear()
		{
			if (this.Objects != null)
			{
				this.Objects.Clear();
				this.Objects = null;
			}

			if (this.TopLeftChild != null)
			{
				this.TopLeftChild.Clear();
				this.TopRightChild.Clear();
				this.BottomLeftChild.Clear();
				this.BottomRightChild.Clear();

				this.TopLeftChild = null;
				this.TopRightChild = null;
				this.BottomLeftChild = null;
				this.BottomRightChild = null;
			}
		}

		public int ObjectCount()
		{
			var count = 0;

			if (this.Objects != null)
				count += this.Objects.Count;

			if (this.TopLeftChild != null)
			{
				count += this.TopLeftChild.ObjectCount();
				count += this.TopRightChild.ObjectCount();
				count += this.BottomLeftChild.ObjectCount();
				count += this.BottomRightChild.ObjectCount();
			}

			return count;
		}

		public void Delete(TObject item)
		{
			bool objectRemoved = false;
			if (this.Objects != null && this.Objects.Contains(item))
			{
				this.Remove(item);
				objectRemoved = true;
			}

			if (this.TopLeftChild != null && !objectRemoved)
			{
				this.TopLeftChild.Delete(item);
				this.TopRightChild.Delete(item);
				this.BottomLeftChild.Delete(item);
				this.BottomRightChild.Delete(item);
			}

			if (this.TopLeftChild != null)
			{
				if (this.TopLeftChild.ObjectCount() == 0 && this.TopRightChild.ObjectCount() == 0 && this.BottomLeftChild.ObjectCount() == 0 && this.BottomRightChild.ObjectCount() == 0)
				{
					this.TopLeftChild = null;
					this.TopRightChild = null;
					this.BottomLeftChild = null;
					this.BottomRightChild = null;
				}
			}
		}

		public void Insert(TObject item)
		{
			if (!this.Rect.IntersectsWith(item.Rect))
				return;

			if (this.Objects == null || (this.TopLeftChild == null && this.Objects.Count + 1 <= MaxObjects))
			{
				this.Add(item);
			}
			else
			{
				if (this.TopLeftChild == null)
					this.Split();

				var destTree = this.GetDestinationTree(item);
				if (destTree == this)
					this.Add(item);
				else
					destTree.Insert(item);
			}
		}

		public void GetObjects(Rectangle rect, ref List<TObject> results)
		{
			if (results != null)
			{
				if (rect.Contains(this.Rect))
				{
					this.GetAllObjects(ref results);
				}
				else if (rect.IntersectsWith(this.Rect))
				{
					if (this.Objects != null)
					{
						for (int i = 0; i < this.Objects.Count; i++)
						{
							if (rect.IntersectsWith(this.Objects[i].Rect))
							{
								results.Add(this.Objects[i]);
							}
						}
					}

					if (this.TopLeftChild != null)
					{
						this.TopLeftChild.GetObjects(rect, ref results);
						this.TopRightChild.GetObjects(rect, ref results);
						this.BottomLeftChild.GetObjects(rect, ref results);
						this.BottomRightChild.GetObjects(rect, ref results);
					}
				}
			}
		}

		public void GetAllObjects(ref List<TObject> results)
		{
			if (this.Objects != null)
				results.AddRange(this.Objects);

			if (this.TopLeftChild != null)
			{
				this.TopLeftChild.GetAllObjects(ref results);
				this.TopRightChild.GetAllObjects(ref results);
				this.BottomLeftChild.GetAllObjects(ref results);
				this.BottomRightChild.GetAllObjects(ref results);
			}
		}
	}
}
