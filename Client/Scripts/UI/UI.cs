using System;
using System.Numerics;

namespace Splat;

public abstract class UI : Entity
{
	bool hover;

	public bool Contains(Vector2 point)
	{
		return (point.X >= transform.x && point.X <= transform.x + transform.w)
			&& (point.Y >= transform.y && point.Y <= transform.y + transform.h);
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