#include "Window.h"

Window::Window(const char* title, int width, int height)
{
	if (SDL_Init(SDL_INIT_EVERYTHING) != 0)
	{
		SDL_GetError();
	}

	window = SDL_CreateWindow(title, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1280, 720, SDL_WINDOW_MAXIMIZED);
	if (window == NULL)
	{
		SDL_GetError();
	}

	context = SDL_GL_CreateContext(window);
	if (context == NULL)
	{
		SDL_GetError();
	}
}