using System.Numerics;

namespace Splat;

public abstract class UI : Entity
{
	bool hover;

	public bool Contains(Vector2 point)
	{
		return (point.X >= Transform.x && point.X <= Transform.x + Transform.w)
			&& (point.Y >= Transform.y && point.Y <= Transform.y + Transform.h);
	}

	public virtual void OnHoverEnter() { }
	public virtual void OnHoverExit() { }

	public override void Update()
	{
		if (Contains(Cursor.Position))
		{
			if (Cursor.State == SDL_PRESSED)
			{
				Cursor.Selection = this;
			}

			if (!hover)
			{
				hover = true;
				OnHoverEnter();
			}
		}
		else
		{
			if (hover)
			{
				hover = false;
				OnHoverExit();
			}
		}
	}
}