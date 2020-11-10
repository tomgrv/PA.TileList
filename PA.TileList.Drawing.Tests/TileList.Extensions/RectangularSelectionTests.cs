
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
	public class RectangularSelectionTests
	{
		private RectangularRenderer rrr = new RectangularRenderer(Color.Black, 1);

		
		[Test]
		public void CoordinatesIn_bloc_IU010_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(153, count, "index 0,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_IU010_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(162, count, "index 0,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_IU025_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(130, count, "index 0,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_IU025_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(143, count, "index 0,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_IU100_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(130, count, "index 0,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_IU100_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(117, count, "index 0,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_Ix010_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(130, count, "index 0,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_Ix010_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(162, count, "index 0,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_Ix025_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(130, count, "index 0,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_Ix025_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(143, count, "index 0,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_Ix100_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(117, count, "index 0,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_Ix100_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(117, count, "index 0,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_xU010_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(36, count, "index 0,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_xU010_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 0,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_xU025_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 0,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_xU025_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 0,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_xU100_0()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
			var scs =  new SelectionConfiguration(SelectionPosition.Under,1f,false);
			
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
					var count = 0;

					scs.OptimizeResolution(tile, pro);

					//foreach (var c in tile.SelectCoordinates(pro, scs))
					foreach (var c in tile.Take(pro, scs))
					{
						tile.Find(c).Context.Color = Color.Brown; 
						count++;
					}

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 0,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_bloc_xU100_1()
		{
			var pro =  new RectangularProfile(-2000,-1990,-1000,-1000,"bloc");
			var scs =  new SelectionConfiguration(SelectionPosition.Under,1f,true);
			
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
					var count = 0;

					scs.OptimizeResolution(tile, pro);

					//foreach (var c in tile.SelectCoordinates(pro, scs))
					foreach (var c in tile.Take(pro, scs))
					{
						tile.Find(c).Context.Color = Color.Brown; 
						count++;
					}

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 0,8,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_IU010_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(41, count, "index 1,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_IU010_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(63, count, "index 1,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_IU025_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(40, count, "index 1,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_IU025_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(41, count, "index 1,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_IU100_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(20, count, "index 1,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_IU100_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(20, count, "index 1,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_Ix010_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(40, count, "index 1,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_Ix010_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(42, count, "index 1,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_Ix025_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(40, count, "index 1,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_Ix025_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(40, count, "index 1,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_Ix100_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(20, count, "index 1,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_Ix100_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 1,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_xU010_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(1, count, "index 1,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_xU010_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(41, count, "index 1,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_xU025_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 1,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_xU025_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 1,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_xU100_0()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
			var scs =  new SelectionConfiguration(SelectionPosition.Under,1f,false);
			
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
					var count = 0;

					scs.OptimizeResolution(tile, pro);

					//foreach (var c in tile.SelectCoordinates(pro, scs))
					foreach (var c in tile.Take(pro, scs))
					{
						tile.Find(c).Context.Color = Color.Brown; 
						count++;
					}

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 1,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_large_xU100_1()
		{
			var pro =  new RectangularProfile(-25,0,2000,112.5,"bande_large");
			var scs =  new SelectionConfiguration(SelectionPosition.Under,1f,true);
			
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
					var count = 0;

					scs.OptimizeResolution(tile, pro);

					//foreach (var c in tile.SelectCoordinates(pro, scs))
					foreach (var c in tile.Take(pro, scs))
					{
						tile.Find(c).Context.Color = Color.Brown; 
						count++;
					}

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 1,8,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_IU010_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(7, count, "index 2,0,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_IU010_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(6, count, "index 2,0,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_IU025_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,1,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_IU025_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,1,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_IU100_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,2,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_IU100_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,2,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_Ix010_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(7, count, "index 2,3,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_Ix010_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(6, count, "index 2,3,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_Ix025_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,4,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_Ix025_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,4,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_Ix100_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,5,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_Ix100_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,5,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_xU010_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,6,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_xU010_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,6,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_xU025_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,7,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_xU025_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
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

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,7,1" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_xU100_0()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
			var scs =  new SelectionConfiguration(SelectionPosition.Under,1f,false);
			
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
					var count = 0;

					scs.OptimizeResolution(tile, pro);

					//foreach (var c in tile.SelectCoordinates(pro, scs))
					foreach (var c in tile.Take(pro, scs))
					{
						tile.Find(c).Context.Color = Color.Brown; 
						count++;
					}

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,8,0" );
		}

		
		[Test]
		public void CoordinatesIn_bande_fine_xU100_1()
		{
			var pro =  new RectangularProfile(-990,-500,-980,0,"bande_fine");
			var scs =  new SelectionConfiguration(SelectionPosition.Under,1f,true);
			
					var tile = MainTile.GetTile(1).Flatten<SubTile, Item>();
					var count = 0;

					scs.OptimizeResolution(tile, pro);

					//foreach (var c in tile.SelectCoordinates(pro, scs))
					foreach (var c in tile.Take(pro, scs))
					{
						tile.Find(c).Context.Color = Color.Brown; 
						count++;
					}

					tile.GetDebugGraphic(pro, rrr, scs).SaveDebugImage();

					Assert.AreEqual(0, count, "index 2,8,1" );
		}

			}
}