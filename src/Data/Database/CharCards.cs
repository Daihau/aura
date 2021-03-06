﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using System.Collections.Generic;

namespace Aura.Data.Database
{
	public class CharCardData
	{
		public int Id { get; internal set; }
		public string Name { get; internal set; }
		public int SetId { get; internal set; }
		public List<int> Races { get; internal set; }

		public CharCardData()
		{
			this.Races = new List<int>();
		}

		public bool Enabled(int race)
		{
			return this.Races.Contains(race);
		}
	}

	/// <summary>
	/// Indexed by char card id.
	/// </summary>
	public class CharCardDb : DatabaseCSVIndexed<int, CharCardData>
	{
		[MinFieldCount(3)]
		protected override void ReadEntry(CSVEntry entry)
		{
			var info = new CharCardData();
			info.Id = entry.ReadInt();
			info.SetId = entry.ReadInt();

			var races = entry.ReadUIntHex();
			if ((races & 0x01) != 0) info.Races.Add(10001);
			if ((races & 0x02) != 0) info.Races.Add(10002);
			if ((races & 0x04) != 0) info.Races.Add(9001);
			if ((races & 0x08) != 0) info.Races.Add(9002);
			if ((races & 0x10) != 0) info.Races.Add(8001);
			if ((races & 0x20) != 0) info.Races.Add(8002);

			this.Entries[info.Id] = info;
		}
	}
}
