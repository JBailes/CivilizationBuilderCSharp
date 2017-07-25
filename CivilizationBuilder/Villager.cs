using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CivilizationBuilder
{
    public class Villager : Actor
    {
		public string sprite;
		public long food;
		public long critical_food;
		public long max_food;
		public long thirst;
		public long critical_thirst;
		public long max_thirst;
		public long fatigue;
		public long critical_fatigue;
		public long max_fatigue;
		long age = 18;

		int sex;

		Path path = null;
		int pathStep = 0;

		Point target;

		const long NOTHING = 0;
		const long EATING = 1;
		const long DRINKING = 2;
		const long RESTING = 3;
		const long WORKING = 4;
		const long PATHING = 5;

		const int SEX_MALE = 1;
		const int SEX_FEMALE = 2;

		public const int xOffset = 32;
		public const int yOffset = 32;

		public const int tileHeight = 500;
		public const int tileWidth = 500;

		public int xFrame = 0;
		public int yFrame = 0;

		public int animationState = 0;

		public Villager(string name, int endurance = 1000, int strength = 1000, int intelligence = 1000)
		{
			if (endurance == 1000)
				endurance = Debug.GetRandomNumber(500, 2500);

			if (strength == 1000)
				strength = Debug.GetRandomNumber(500, 2500);

			if (intelligence == 1000)
				intelligence = Debug.GetRandomNumber(500, 2500);

			if (Debug.GetRandomNumber(1, 1000) < 500)
				sex = SEX_MALE;
			else
				sex = SEX_FEMALE;

			sprite = "sprite sheet";

			this.name = name;
			this.endurance = endurance;
			this.strength = strength;
			this.intelligence = intelligence;

			recalculateStats();

			food = max_food;
			thirst = max_thirst;
			fatigue = max_fatigue;

			x = 9*32;
			y = 1*32;

			update();
		}

		public void update()
		{
			animationState = 0;
			if (food < critical_food*2)
			{
				if (World.world[x / 32, y / 32] == World.TILE_FOOD)
					setState(EATING);
				else
				{
					target = new Point(19, 19);
					setState(PATHING);
				}
			}
			else if (thirst < critical_thirst*2)
			{
				if (World.world[x / 32, y / 32] == World.TILE_WATER)
					setState(DRINKING);
				else
				{
					target = new Point(19, 1);
					setState(PATHING);
				}
			}
			else if (fatigue < critical_fatigue*2)
			{
				if (World.world[x / 32, y / 32] == World.TILE_REST)
					setState(RESTING);
				else
				{
					target = new Point(1, 1);
					setState(PATHING);
				}
			}
			else
			{
				if (World.world[x / 32, y / 32] == World.TILE_WORK)
					setState(WORKING);
				else
				{
					target = new Point(1, 19);
					setState(PATHING);
				}
			}

			if (state != PATHING)
			{
				pathStep = 0;
				path = null;
			}
		}

		public void tick (long timelapse)
		{
			animationState++;
			food -= timelapse;
			thirst -= timelapse;
			fatigue -= timelapse;

			statehandler(timelapse);
		}

		public void interrupt(long timelapse)
		{
			// Interrupted due to situation change.
			//update();
		}

		private void statehandler(long timelapse)
		{
			switch(state)
			{
				case EATING:
					animationState = animationState % 30;

					if (animationState > 15)
					{
						xFrame = 2;
						yFrame = 1;
					}
					else
					{
						xFrame = 0;
						yFrame = 0;
					}
					eat(timelapse);
					break;
				case DRINKING:
					animationState = animationState % 30;

					if (animationState > 15)
					{
						xFrame = 2;
						yFrame = 1;
					}
					else
					{
						xFrame = 0;
						yFrame = 0;
					}
					drink(timelapse);
					break;
				case RESTING:
					xFrame = 0;
					yFrame = 1;
					rest(timelapse);
					break;
				case WORKING:
					animationState = animationState % 30;

					if (animationState > 15)
					{
						yFrame = 1;
						xFrame = 1;
					}
					else
					{
						xFrame = 0;
						yFrame = 0;
					}
					work(timelapse);
					break;
				case PATHING:
					animationState = animationState % 60;
					if (animationState > 30)
						xFrame = 2;
					else
						xFrame = 1;
					yFrame = 0;
					pathToTarget(timelapse);
					break;
				default:
					break;
			}
		}

		private void pathToTarget(long timelapse)
		{
			if (path == null)
			{
				path = PathFind.FastGeneratePath(new Point(x/32, y/32), target);
			}
			
			var nextStep = path.curPath[pathStep];

			if (x/32 == nextStep.X && y/32 == nextStep.Y)
			{
				pathStep++;

				if (pathStep >= path.curPath.Count)
				{
					path = null;
					pathStep = 0;
					setState(NOTHING);
					return;
				}

				nextStep = path.curPath[pathStep];
			}

			if (x / 32 < nextStep.X)
				x++;
			else if (x / 32 > nextStep.X)
				x--;

			if (y / 32 < nextStep.Y)
				y++;
			else if (y / 32 > nextStep.Y)
				y--;

			if (x / 32 == target.X && y / 32 == target.Y)
			{
				path = null;
				pathStep = 0;
				setState(NOTHING);
			}
		}

		private void eat(long timelapse)
		{
			food += timelapse * 20;

			if (food > max_food)
				setState(NOTHING);
		}

		private void drink(long timelapse)
		{
			thirst += timelapse * 30;

			if (thirst > max_thirst)
				setState(NOTHING);
		}

		private void rest(long timelapse)
		{
			fatigue += timelapse * 4;

			if (fatigue > max_fatigue)
				setState(NOTHING);
		}

		private void work(long timelapse)
		{
		}

		private void setState(long stateToSet)
		{
			switch(stateToSet)
			{
				case EATING:
					state = EATING;
					break;
				case DRINKING:
					state = DRINKING;
					break;
				case RESTING:
					state = RESTING;
					break;
				case PATHING:
					state = PATHING;
					break;
				case WORKING:
					state = WORKING;
					break;
				default:
					state = NOTHING;
					break;
			}
		}

		private void recalculateStats()
		{
			max_food = 10000 + endurance * 100;
			critical_food = max_food / 10;

			max_thirst = 8000 + endurance * 20;
			critical_thirst = max_thirst / 10;

			max_fatigue = 10000 + endurance * 200;
			critical_fatigue = max_fatigue / 10;
		}
	}
}
