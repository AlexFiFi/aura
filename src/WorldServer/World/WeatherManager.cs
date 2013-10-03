// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.Player;
using Aura.Data;

namespace Aura.World.World
{
	/// <summary>
	/// Manages regions ids and the relevant weather descriptions.
	/// </summary>
	public class WeatherManager
	{
		/// <summary>
		/// Time it takes to go from one weather to the next.
		/// (Basically how long it will take for the clouds to form.)
		/// </summary>
		private const uint Transition = 60000;

		public readonly static WeatherManager Instance = new WeatherManager();

		private Dictionary<uint, WeatherDesc> _weather = new Dictionary<uint, WeatherDesc>();
		private Dictionary<uint, float> _prevWeather = new Dictionary<uint, float>();

		private WeatherManager()
		{
		}

		/// <summary>
		/// Loads weather and subscribes to relevant events.
		/// </summary>
		public void Init()
		{
			this.LoadWeather();

			EventManager.TimeEvents.RealTimeTick += this.OnRealTimeTick;
			EventManager.PlayerEvents.PlayerChangesRegion += this.OnPlayerChangesRegion;
		}

		/// <summary>
		/// Sets weather, using information from MabiData.
		/// </summary>
		private void LoadWeather()
		{
			foreach (var entry in MabiData.WeatherDb.Entries)
			{
				switch (entry.Value.Type)
				{
					case WeatherInfoType.Official: // Not supported yet
					case WeatherInfoType.Custom:
						this.SetWeather(entry.Key, new WeatherDescSeed((uint)entry.Value.Values[0]));
						break;
					case WeatherInfoType.Pattern:
						this.SetWeather(entry.Key, new WeatherDescPattern(entry.Value.Values.ToArray()));
						break;
					case WeatherInfoType.OWM:
						this.SetWeather(entry.Key, new WeatherDescOWM((uint)entry.Value.Values[0]));
						break;
				}
			}
		}

		/// <summary>
		/// Sets the weather description for this region. Weather doesn't
		/// change immediately, but at the next interval.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="wd"></param>
		public void SetWeather(uint region, WeatherDesc wd)
		{
			_weather[region] = wd;
		}

		/// <summary>
		/// Sets weather to a new pattern, only consisting of value.
		/// It will be updated immediately, and stay till another manual
		/// change, or server restart.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="value"></param>
		public void SetWeather(uint region, float value)
		{
			_weather[region] = new WeatherDescPattern(value);
			WorldManager.Instance.BroadcastRegion(this.GetWeatherPacket(region, value, value, 0), region);
		}

		/// <summary>
		/// Returns weather description for the region.
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		public WeatherDesc GetWeatherDesc(uint region)
		{
			return _weather[region];
		}

		private void OnRealTimeTick(MabiTime time)
		{
			if (time.DateTime.Minute % 20 != 0 && _prevWeather.Count > 0)
				return;

			this.BroadcastWeather();
		}

		/// <summary>
		/// Sends the current weather for the region the player is moving to.
		/// Sends empty weather packet if no weather description could be found.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnPlayerChangesRegion(MabiPC character)
		{
			if (character == null)
				return;

			if (_weather.ContainsKey(character.Region))
			{
				var weather = _weather[character.Region].GetWeather(DateTime.Now);
				character.Client.Send(this.GetWeatherPacket(character.Region, 0.5f, weather, 0));
			}
			else
			{
				// The second int is unknown, but this is expected
				// to reset the weather.
				var p = new MabiPacket(Op.Weather, Id.Broadcast);
				p.PutByte(0);
				p.PutInt(character.Region);
				p.PutByte(0);
				p.PutInt(10000);
				p.PutByte(0);
				character.Client.Send(p);
			}
		}

		/// <summary>
		/// Broadcasts current weather data to all characters.
		/// </summary>
		private void BroadcastWeather()
		{
			foreach (var w in _weather)
			{
				var weather = w.Value.GetWeather(DateTime.Now);

				// Get new weather and save current weather for next transition.
				var from = (_prevWeather.ContainsKey(w.Key) ? _prevWeather[w.Key] : 0.5f);
				var to = (_prevWeather[w.Key] = weather);

				WorldManager.Instance.BroadcastRegion(this.GetWeatherPacket(w.Key, from, to), w.Key);

				//Logger.Debug("Current weather in {0}: {1} ({2})", w.Key, w.Value.GetWeatherType(DateTime.Now), weather);
			}
		}

		public MabiPacket GetWeatherPacket(uint region, float from, float to, uint transitionTime = Transition)
		{
			var p = new MabiPacket(Op.Weather, Id.Broadcast);
			p.PutByte(0);
			p.PutInt(region);
			p.PutByte(2);
			p.PutByte(0);
			p.PutByte(1);
			p.PutString("constant_smooth");
			p.PutFloat(to);
			p.PutLong(DateTime.Now);
			p.PutLong(DateTime.MinValue);
			p.PutFloat(from);
			p.PutFloat(from);
			p.PutLong(transitionTime);
			p.PutByte(false);
			p.PutLong(DateTime.MinValue);
			p.PutInt(2);
			p.PutByte(0);

			return p;
		}
	}

	/// <summary>
	/// Bascic class for weather descriptions, based on w/e.
	/// </summary>
	public abstract class WeatherDesc
	{
		/// <summary>
		/// New weather every 1200s (20 minutes).
		/// </summary>
		protected const int Interval = 1200;

		protected const float DefaultWeather = 0.5f;

		/// <summary>
		/// Base time for calcuating time passed in GetWeather.
		/// </summary>
		protected readonly DateTime DawnOfTime = new DateTime(2013, 1, 1);

		protected int _cacheIdx;
		protected float _cache;

		/// <summary>
		/// Returns weather for the current interval (0-19, 20-39, 40-59)
		/// as float between 0.0 and 2.0 (clear and stormy).
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public abstract float GetWeather(DateTime dt);

		/// <summary>
		/// Returns weather intervals (20min by default) between DawnOfTime
		/// and the given DateTime.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		protected int GetPassedIntervals(DateTime dt)
		{
			return (int)(dt - DawnOfTime).TotalSeconds / Interval;
		}

		/// <summary>
		/// Returns the type of the current weather.
		/// </summary>
		public WeatherType GetWeatherType(DateTime dt)
		{
			var weather = this.GetWeather(dt);
			if (weather < 1)
				return WeatherType.Clear;
			if (weather < 1.95)
				return WeatherType.Cloudy;
			if (weather < 2)
				return WeatherType.Rain;

			return WeatherType.Thunderstorm;
		}

		/// <summary>
		/// Returns current rain strength (0~100);
		/// </summary>
		public byte GetRainStrength(DateTime dt)
		{
			var weather = this.GetWeather(dt);
			if (weather < 1.95)
				return 0;
			return (byte)(100f / 0.5f * (2 - weather));
		}
	}

	/// <summary>
	/// Custom, seed-based weather description. Weather is generated randomly
	/// based on the given seed.
	/// </summary>
	public class WeatherDescSeed : WeatherDesc
	{
		protected uint _seed;

		public WeatherDescSeed(uint seed)
		{
			_seed = seed;
		}

		public override float GetWeather(DateTime dt)
		{
			int passed = this.GetPassedIntervals(dt);
			if (_cacheIdx == passed)
				return _cache;

			// Skip till we get to the current time
			var rnd = new MTRandom(_seed);
			for (int i = 0; i < passed; ++i)
				rnd.GetUInt32();

			// Cache
			_cacheIdx = passed;

			// Generate
			// Ratio similar to official.
			var r = rnd.GetUInt32(1, 10000);
			// 0.92%
			if (r <= 92)
			{
				_cache = 2.00f;
			}
			// 16.60%
			else if (r <= 92 + 1660)
			{
				var n = 100f / (92 + 1660) * r / 100f;
				_cache = 1.95f + 0.04f * n;
			}
			// 24.50%
			else if (r <= 92 + 1660 + 2450)
			{
				var n = 100f / (92 + 1660 + 2450) * r / 100f;
				_cache = 1f + 0.9f * n;
			}
			// 57.98%
			else
			{
				_cache = 0.50f;
			}

			return _cache;
		}
	}

	/// <summary>
	/// Custom, pattern-based weather description. Weather is generated based
	/// on the given pattern (constantly repeating).
	/// </summary>
	public class WeatherDescPattern : WeatherDesc
	{
		protected List<float> _pattern;

		public WeatherDescPattern(params float[] weathers)
		{
			_pattern = new List<float>();
			_pattern.AddRange(weathers);
		}

		public override float GetWeather(DateTime dt)
		{
			int passed = this.GetPassedIntervals(dt);
			if (_cacheIdx == passed)
				return _cache;

			_cacheIdx = passed;
			_cache = _pattern[passed % _pattern.Count];

			return _cache;
		}
	}

	/// <summary>
	/// Weather description using Open Weather Map (http://openweathermap.org)
	/// to get the current weather in the city with the id passed to the constructor.
	/// </summary>
	public class WeatherDescOWM : WeatherDesc
	{
		private uint _cityId;

		public WeatherDescOWM(uint cityId)
		{
			_cityId = cityId;
		}

		public override float GetWeather(DateTime dt)
		{
			int passed = this.GetPassedIntervals(dt);
			if (_cacheIdx == passed)
				return _cache;

			var down = new WebClient().DownloadString("http://api.openweathermap.org/data/2.5/weather?id=" + _cityId.ToString() + "&mode=xml");
			var match = Regex.Match(down, "<weather number=\"([0-9]+)\" value");
			if (match.Success)
			{
				var code = Convert.ToUInt32(match.Groups[1].Value);
				if ((code >= 200 && code <= 232) || (code >= 900 && code <= 906))
				{
					_cache = 2f;
				}
				else if ((code >= 300 && code <= 321) || (code >= 600 && code <= 601))
				{
					_cache = 1.95f;
				}
				else if ((code >= 500 && code <= 522) || (code >= 602 && code <= 621))
				{
					_cache = 1.99f;
				}
				else if (code == 800)
				{
					_cache = 0.1f;
				}
				else if (code == 801)
				{
					_cache = 0.5f;
				}
				else if (code == 802)
				{
					_cache = 1.0f;
				}
				else if (code == 803)
				{
					_cache = 1.5f;
				}
				else if (code == 804)
				{
					_cache = 1.9f;
				}
				else if (code == 31)
				{
					_cache = 1.95f;
				}
				else
					_cache = DefaultWeather;
			}
			else
				_cache = DefaultWeather;

			return _cache;
		}
	}

	public enum WeatherType
	{
		Clear, Cloudy, Rain, Thunderstorm
	}
}
