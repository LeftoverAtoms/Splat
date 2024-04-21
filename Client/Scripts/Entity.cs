namespace Splat;

public abstract class Entity
{
	public SDL_Rect Transform;

	public Entity()
	{
		Game.Entities.Add(this);
	}

	public void SetPosition(int x, int y)
	{
		Transform.x = x;
		Transform.y = y;
	}
	public void SetScale(int w, int h)
	{
		Transform.w = w;
		Transform.h = h;
	}

	public virtual void Start() { }
	public virtual void Update() { }
	public virtual void Render() { }
	public virtual void Event(SDL_Event evt) { }
}