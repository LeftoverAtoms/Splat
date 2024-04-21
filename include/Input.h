#pragma once

#include <SDL2/SDL.h>

struct Input
{
	static bool KeyCode[SDL_NUM_SCANCODES];

	static bool Pressed(SDL_KeyCode keycode);
};