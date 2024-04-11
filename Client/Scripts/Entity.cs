namespace Splat;

public abstract class Entity
{
	public SDL_Rect Transform { get => transform; }

	#region Backing Fields
	protected SDL_Rect transform;
	#endregion

	public Entity()
	{
		Program.Entities.Add(this);
	}

	public void SetPosition(int x, int y)
	{
		transform.x = x;
		transform.y = y;
	}
	public void SetScale(int w, int h)
	{
		transform.w = w;
		transform.h = h;
	}

	public virtual void Update() { }
	public virtual void Render() { }
	public virtual void OnEvent(SDL_Event e) { }
}