<?php

class ErrorView extends View
{
	public function __construct(
		public int $code = 404,
		public bool $isAPI = true,
		public ?string $message = null
	){}

	public function render(): void {
		if ($this->message === null) {
			$this->message = match($this->code) {
				100 => 'Continue',
				101 => 'Switching Protocols',
				200 => 'OK',
				201 => 'Created',
				202 => 'Accepted',
				203 => 'Non-Authoritative Information',
				204 => 'No Content',
				205 => 'Reset Content',
				206 => 'Partial Content',
				300 => 'Multiple Choices',
				301 => 'Moved Permanently',
				302 => 'Moved Temporarily',
				303 => 'See Other',
				304 => 'Not Modified',
				305 => 'Use Proxy',
				400 => 'Bad Request',
				401 => 'Unauthorized',
				402 => 'Payment Required',
				403 => 'Forbidden',
				404 => 'Not Found',
				405 => 'Method Not Allowed',
				406 => 'Not Acceptable',
				407 => 'Proxy Authentication Required',
				408 => 'Request Time-out',
				409 => 'Conflict',
				410 => 'Gone',
				411 => 'Length Required',
				412 => 'Precondition Failed',
				413 => 'Request Entity Too Large',
				414 => 'Request-URI Too Large',
				415 => 'Unsupported Media Type',
				418 => 'I\'m a teapot',
				451 => 'Unavailable For Legal Reasons',
				500 => 'Internal Server Error',
				501 => 'Not Implemented',
				502 => 'Bad Gateway',
				503 => 'Service Unavailable',
				504 => 'Gateway Time-out',

				// Custom error codes
				// Login
				480 => 'Wrong credentials',
				481 => 'E-Mail not confirmed',
				482 => 'User does not exist',
				// Register
				486 => 'E-Mail already taken',
				default => 'Undefined error code'
			};
		}

		http_response_code($this->code);									// Set the correct HTML response code
		if ($this->isAPI) {
			(new APIView([$this->code => $this->message]))->render();		// Render error as JSON
		} else {
			echo "<h1>" . $this->code . " - " . $this->message . "</h1>";	// Render error as HTML
			$this->renderChildren();
		}
	}
}
