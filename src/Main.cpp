#include "Input.h"
#include "Window.h"

#include <SDL2/SDL.h>

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
				Input::KeyCode[event.key.keysym.sym] = true;
			if (event.type == SDL_KEYUP)
				Input::KeyCode[event.key.keysym.sym] = false;
		}
	}

	return 0;
}