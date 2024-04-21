#pragma once

#include <SDL2/SDL.h>

class Window
{
public:
	Window(const char* title, int width, int height);

private:
	SDL_Window* window;
	SDL_GLContext context;
};