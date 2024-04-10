using SDL2;
using System;

namespace Splat;

class Program
{
	static nint Window;
	static nint Renderer;

	static void Main(string[] args)
	{
		// Connect a client to the server.
		var client = new Client("Adam");
		client.Connect("127.0.0.1", 1234);

		SetupSDL();

		SDL_ttf.TTF_Init();
		IntPtr font = SDL_ttf.TTF_OpenFont("Roboto-Regular.ttf", 24);
		if (font == IntPtr.Zero)
		{
			Console.WriteLine("Failed to load font");
			return;
		}

		SDL.SDL_Color textColor = new SDL.SDL_Color { r = 255, g = 255, b = 255 };

		IntPtr surface = SDL_ttf.TTF_RenderText_Solid(font, client.Data.Name, textColor);
		if (surface == IntPtr.Zero)
		{
			Console.WriteLine("Failed to render text surface");
			return;
		}

		IntPtr texture = SDL.SDL_CreateTextureFromSurface(Renderer, surface);
		if (texture == IntPtr.Zero)
		{
			Console.WriteLine("Failed to create texture from surface");
			return;
		}

		SDL.SDL_FreeSurface(surface);

		SDL.SDL_Rect destRect = new SDL.SDL_Rect { x = 100, y = 100, w = 64, h = 32 };

		SDL.SDL_RenderCopy(Renderer, texture, IntPtr.Zero, ref destRect);

		SDL.SDL_RenderPresent(Renderer);

		while (true)
		{
			while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
			{
				switch (e.type)
				{
					case SDL.SDL_EventType.SDL_QUIT:
					return;
				}
			}
		}
	}

	static void SetupSDL()
	{
		SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

		Window = SDL.SDL_CreateWindow("Splat", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 1280, 720, SDL.SDL_WindowFlags.SDL_WINDOW_MAXIMIZED);

		Renderer = SDL.SDL_CreateRenderer(Window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
	}
}