#include "Window.h"

#include <SDL2/SDL.h>

unsigned short keycodes[SDL_NUM_SCANCODES];

int main(int argc, char* argv[])
{
	auto window = Window("Splat", 1280, 720);

	SDL_Event event;

	bool isRunning = true;

	while (isRunning)
	{
		while (SDL_PollEvent(&event) != 0)
		{
			if (event.type == SDL_QUIT)
				isRunning = false;
			if (event.type == SDL_KEYDOWN)
				keycodes[event.key.keysym.scancode] = 1;
			if (event.type == SDL_KEYUP)
				keycodes[event.key.keysym.scancode] = 0;
		}
	}

	return 0;
}