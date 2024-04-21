#include "Input.h"

bool Input::KeyCode[SDL_NUM_SCANCODES];

bool Input::Pressed(SDL_KeyCode keycode)
{
	return Input::KeyCode[keycode];
}