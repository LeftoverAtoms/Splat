global using static SDL2.SDL;
global using static SDL2.SDL_image;
global using static SDL2.SDL_ttf;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Splat;

public class Game
{
	const string TITLE = "Splat";
	const string FONT = "Roboto-Regular.ttf";

	public static List<Entity> Entities { get; } = new List<Entity>(10);

	public static IntPtr Window { get; private set; }
	public static IntPtr Renderer { get; private set; }

	public static IntPtr Font { get; private set; }

	static bool running = true;

	public static float DeltaTime { get; private set; }

	public static Client? LocalClient { get; } = new Client();

	static void Main()
	{
		bool succeeded = SetupSDL();

		if (succeeded)
		{
			Start();

			float b = 0;

			// Execute gameloop.
			while (running)
			{
				float a = SDL_GetTicks();
				DeltaTime = a - b;

				if (DeltaTime > 1000 / 60.0f)
				{
					b = a;

					Input();
					Update();
					Render();
				}
			}
		}

		// Exit application.
		Quit();
	}

	static bool SetupSDL()
	{
		// Init SDL.
		if (SDL_Init(SDL_INIT_EVERYTHING) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Init SDL image.
		IMG_InitFlags img_initflags = IMG_InitFlags.IMG_INIT_PNG;
		if (IMG_Init(img_initflags) != (int)img_initflags)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Init SDL TTF.
		if (TTF_Init() != 0)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Create window.
		Window = SDL_CreateWindow(TITLE, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1280, 720, SDL_WindowFlags.SDL_WINDOW_MAXIMIZED);
		if (Window == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Create renderer.
		Renderer = SDL_CreateRenderer(Window, 0, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
		if (Renderer == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Open font.
		Font = TTF_OpenFont(FONT, 24);
		if (Font == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Success!
		return true;
	}

	static void Start()
	{
		_ = new Label(128, 32, "Splat");

		_ = new LabelField(128, 128, "Label Field 1");
		_ = new LabelField(512, 128, "Label Field 2");

		new Image(256, 256, 128, 90, "Arrow.png").SetColorTint(255, 0, 255);
		new Image(384, 256, 128, 180, "Arrow.png").SetColorTint(0, 255, 255);
		new Image(512, 256, 128, 0, "Arrow.png").SetColorTint(0, 255, 0);
		new Image(640, 256, 128, 270, "Arrow.png").SetColorTint(255, 0, 0);

		foreach (var entity in Entities)
		{
			entity.Start();
		}
	}

	static void Input()
	{
		Cursor.State = SDL_RELEASED;

		// Handle all events within the queue before continuing.
		while (SDL_PollEvent(out var evt) != 0)
		{
			Cursor.Selection?.Event(evt);

			switch (evt.type)
			{
				case SDL_EventType.SDL_MOUSEMOTION:
				{
					Cursor.Position = new Vector2(evt.motion.x, evt.motion.y);
					break;
				}

				case SDL_EventType.SDL_MOUSEBUTTONDOWN:
				{
					Cursor.State = evt.button.state;
					break;
				}

				case SDL_EventType.SDL_QUIT:
				{
					running = false;
					break;
				}
			}
		}
	}
	static void Update()
	{
		foreach (var entity in Entities)
		{
			entity.Update();
		}
	}
	static void Render()
	{
		if (SDL_RenderClear(Renderer) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		foreach (var entity in Entities)
		{
			entity.Render();
		}

		SDL_RenderPresent(Renderer);
	}

	static void Quit()
	{
		SDL_DestroyWindow(Window);
		SDL_DestroyRenderer(Renderer);
		TTF_CloseFont(Font);
	}
}