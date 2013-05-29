﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aura.World.Scripting
{

	public class DialogElement
	{
		public List<DialogElement> Children = new List<DialogElement>();

		public DialogElement Add(params DialogElement[] elements)
		{
			this.Children.AddRange(elements);
			return this;
		}

		public virtual void Render(ref StringBuilder sb)
		{
			foreach (var child in Children)
				child.Render(ref sb);
		}
	}

	/// <summary>
	/// Simple text. Strings passed to Msg are converted into this.
	/// </summary>
	public class DialogText : DialogElement
	{
		public string Text { get; set; }

		public DialogText(string format, params object[] args)
		{
			this.Text = string.Format(format, args);
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.Append(this.Text);

			base.Render(ref sb);
		}
	}

	// Changes the NPC portrait, displayed at the upper left of the dialog.
	public class DialogPortrait : DialogElement
	{
		public string Text { get; set; }

		public DialogPortrait(string text)
		{
			if (text == null)
				this.Text = "NONE";
			else
				this.Text = text;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<npcportrait name='{0}' />", this.Text);

			base.Render(ref sb);
		}
	}

	/// <summary>
	/// Changes the name of the speaking person (at the top of the dialog).
	/// </summary>
	public class DialogTitle : DialogElement
	{
		public string Text { get; set; }

		public DialogTitle(string text)
		{
			if (text == null)
				this.Text = "NONE";
			else
				this.Text = text;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<title name='{0}' />", this.Text);

			base.Render(ref sb);
		}
	}

	/// <summary>
	/// Shows the configured hotkey for the given id.
	/// </summary>
	public class DialogHotkey : DialogElement
	{
		public string Text { get; set; }

		public DialogHotkey(string text)
		{
			this.Text = text;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<hotkey name='{0}' />", this.Text);

			base.Render(ref sb);
		}
	}

	/// <summary>
	/// A button is displayed at the bottom of the dialog, and can be clicked.
	/// The keyword of the button is sent to the server and can be read using Select.
	/// </summary>
	public class DialogButton : DialogElement
	{
		public string Text { get; set; }
		public string Keyword { get; set; }
		public string OnFrame { get; set; }

		public DialogButton(string text, string keyword = null, string onFrame = null)
		{
			this.Text = text;
			this.OnFrame = onFrame;

			if (keyword != null)
				this.Keyword = keyword;
			else
			{
				// Take text, prepend @, replace all non-numerics with _ and
				// make the string lower case, if no keyword was given.
				// Yea... hey, this is 10 times faster than Regex + ToLower!
				var sb = new StringBuilder();
				sb.Append('@');
				foreach (var c in text)
				{
					if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z'))
						sb.Append(c);
					else if (c >= 'A' && c <= 'Z')
						sb.Append((char)(c + 32));
					else
						sb.Append('_');
				}
				this.Keyword = sb.ToString();
			}
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<button title='{0}' keyword='{1}'", this.Text, this.Keyword);
			if (this.OnFrame != null)
				sb.AppendFormat(" onframe='{0}'", this.OnFrame);
			sb.Append(" />");
		}
	}

	/// <summary>
	/// Changes the background music, for the duration of the dialog.
	/// </summary>
	public class DialogBgm : DialogElement
	{
		public string File { get; set; }

		public DialogBgm(string file)
		{
			this.File = file;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<music name='{0}'/>", this.File);
		}
	}

	/// <summary>
	/// Shows an image in the center of the screen.
	/// </summary>
	public class DialogImage : DialogElement
	{
		public string File { get; set; }
		public bool Localize { get; set; }
		public uint Width { get; set; }
		public uint Height { get; set; }

		public DialogImage(string name, bool localize = false, uint width = 0, uint height = 0)
		{
			this.File = name;
			this.Localize = localize;
			this.Width = width;
			this.Height = height;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<image name='{0}'", this.File);
			if (this.Localize)
				sb.Append(" local='true'");
			if (this.Width != 0)
				sb.AppendFormat(" width='{0}'", this.Width);
			if (this.Height != 0)
				sb.AppendFormat(" height='{0}'", this.Height);

			sb.Append(" />");
		}
	}

	/// <summary>
	/// Displays a list of options (buttons) in a separate window.
	/// Result is sent like a regular button click.
	/// </summary>
	public class DialogList : DialogElement
	{
		public string Text { get; set; }
		public string CancelKeyword { get; set; }
		public uint Height { get; set; }

		public DialogList(string text, uint height, string cancelKeyword, params DialogButton[] elements)
		{
			this.Height = height;
			this.Text = text;
			this.CancelKeyword = cancelKeyword;
			this.Add(elements);
		}

		public DialogList(string text, params DialogButton[] elements)
			: this(text, (uint)elements.Length, elements)
		{ }

		public DialogList(string text, uint height, params DialogButton[] elements)
			: this(text, height, "@end", elements)
		{ }

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<listbox page_size='{0}' title='{1}' cancel='{2}'>", this.Height, this.Text, this.CancelKeyword);
			base.Render(ref sb);
			sb.Append("</listbox>");
		}
	}

	/// <summary>
	/// Shows a single lined input box. The result is sent as a regular
	/// Select result.
	/// </summary>
	public class DialogInput : DialogElement
	{
		public string Title { get; set; }
		public string Text { get; set; }
		public byte MaxLength { get; set; }
		public bool Cancelable { get; set; }

		public DialogInput(string title = "Input", string text = "", byte maxLength = 20, bool cancelable = true)
		{
			this.Title = title;
			this.Text = text;
			this.MaxLength = maxLength;
			this.Cancelable = cancelable;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<inputbox title='{0}' caption='{1}' max_len='{2}' allow_cancel='{3}' />", this.Title, this.Text, this.MaxLength, this.Cancelable);

			base.Render(ref sb);
		}
	}

	/// <summary>
	/// Dialog automatically continues after x ms.
	/// </summary>
	public class DialogAutoContinue : DialogElement
	{
		public uint Duration { get; set; }

		public DialogAutoContinue(uint duration)
		{
			this.Duration = duration;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<autopass duration='{0}'/>", this.Duration);
		}
	}

	/// <summary>
	/// Changes the facial expression of the portrait.
	/// (Defined client sided in the db/npc/npcportrait_ani_* files.)
	/// </summary>
	public class DialogFace : DialogElement
	{
		public string Expression { get; set; }

		public DialogFace(string expression)
		{
			this.Expression = expression;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<face name='{0}'/>", this.Expression);
		}
	}

	/// <summary>
	/// Plays a movie in a box in the center of the screen.
	/// Files are taken from movie/.
	/// </summary>
	public class DialogMovie : DialogElement
	{
		public string File { get; set; }
		public uint Width { get; set; }
		public uint Height { get; set; }
		public bool Loop { get; set; }

		public DialogMovie(string file, uint width, uint height, bool loop = true)
		{
			this.File = file;
			this.Width = width;
			this.Height = height;
			this.Loop = loop;
		}

		public override void Render(ref StringBuilder sb)
		{
			sb.AppendFormat("<movie name='{0}' width='{1}' height='{2}' loop='{3}' />", this.File, this.Width, this.Height, this.Loop);
		}
	}
}
