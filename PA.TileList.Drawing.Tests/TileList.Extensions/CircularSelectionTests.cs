
using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using PA.TileList.Circular;
using PA.TileList.Contextual;
using PA.TileList.Cropping;
using PA.TileList.Drawing.Circular;
using PA.TileList.Drawing.Graphics2D;
using PA.TileList.Drawing.Linear;
using PA.TileList.Drawing.Quantified;
using PA.TileList.Linear;
using PA.TileList.Quantified;
using PA.TileList.Selection;
using PA.TileList.Tests.Utils;
using PA.TileList.Tile;

namespace PA.TileList.Drawing.Tests.TileList.Extensions
{

	[TestFixture]
	public class CircularSelectionTests
	{
		private CircularProfileRenderer cpr = new CircularProfileRenderer(Pens.Red, Pens.Red, Pens.Pink);

		
		[Test]
		public void CoordinatesIn_GetTestProfile_IU010_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(411, count, "index 0,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_IU010_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(427, count, "index 0,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_IU025_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(392, count, "index 0,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_IU025_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(406, count, "index 0,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_IU100_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(345, count, "index 0,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_IU100_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(315, count, "index 0,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_Ix010_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(411, count, "index 0,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_Ix010_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(427, count, "index 0,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_Ix025_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(392, count, "index 0,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_Ix025_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(406, count, "index 0,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_Ix100_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(345, count, "index 0,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_Ix100_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(315, count, "index 0,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_xU010_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 0,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_xU010_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 0,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_xU025_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 0,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_xU025_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 0,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_xU100_0()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 0,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetTestProfile_xU100_1()
		{
			var pro =  CircularTests.GetTestProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 0,8,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_IU010_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(314, count, "index 1,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_IU010_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(337, count, "index 1,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_IU025_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(295, count, "index 1,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_IU025_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(308, count, "index 1,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_IU100_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(230, count, "index 1,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_IU100_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(194, count, "index 1,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_Ix010_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(314, count, "index 1,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_Ix010_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(337, count, "index 1,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_Ix025_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(295, count, "index 1,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_Ix025_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(308, count, "index 1,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_Ix100_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(230, count, "index 1,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_Ix100_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(194, count, "index 1,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_xU010_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 1,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_xU010_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 1,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_xU025_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 1,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_xU025_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 1,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_xU100_0()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 1,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetZeroProfile_xU100_1()
		{
			var pro =  CircularTests.GetZeroProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 1,8,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_IU010_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(153, count, "index 2,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_IU010_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(162, count, "index 2,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_IU025_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(295, count, "index 2,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_IU025_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(306, count, "index 2,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_IU100_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(130, count, "index 2,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_IU100_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(117, count, "index 2,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_Ix010_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(130, count, "index 2,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_Ix010_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(162, count, "index 2,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_Ix025_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(130, count, "index 2,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_Ix025_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(143, count, "index 2,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_Ix100_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(117, count, "index 2,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_Ix100_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(117, count, "index 2,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_xU010_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(36, count, "index 2,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_xU010_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 2,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_xU025_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 2,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_xU025_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 2,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_xU100_0()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 2,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetFlatProfile_xU100_1()
		{
			var pro =  CircularTests.GetFlatProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 2,8,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_IU010_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(153, count, "index 3,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_IU010_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(162, count, "index 3,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_IU025_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(295, count, "index 3,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_IU025_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(306, count, "index 3,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_IU100_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(130, count, "index 3,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_IU100_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside | SelectionPosition.Under,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(117, count, "index 3,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_Ix010_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(130, count, "index 3,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_Ix010_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(162, count, "index 3,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_Ix025_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(130, count, "index 3,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_Ix025_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(143, count, "index 3,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_Ix100_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(117, count, "index 3,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_Ix100_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(117, count, "index 3,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_xU010_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(36, count, "index 3,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_xU010_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.10f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 3,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_xU025_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 3,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_xU025_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Under,0.25f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 3,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_xU100_0()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,false);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 3,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_GetSimpleProfile_xU100_1()
		{
			var pro =  CircularTests.GetSimpleProfile(1000);
			var scs =  new SelectionConfiguration(SelectionPosition.Inside ^ SelectionPosition.Outside ,1f,true);
			
			var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
			var count = 0;

			scs.OptimizeResolution(tile, pro);

			//foreach (var c in tile.SelectCoordinates(pro, scs))
			foreach (var c in tile.Take(pro, scs))
			{
				tile.Find(c).Context.Color = Color.Brown; 
				count++;
			}

			tile.GetDebugGraphic(pro, cpr , scs).SaveDebugImage();

			Assert.AreEqual(0, count, "index 3,8,1" );
		}

			}
}