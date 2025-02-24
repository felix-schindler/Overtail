<?php

class PlayerDataController extends Controller
{
	protected array $paths = ['/playerData'];
	protected bool $userRequired = true;

	protected function execute(): void {
		$uuid = Auth::tokenUuid();
		$inv = (new Query('SELECT `item` id, `amount` FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => $uuid]))->fetchAll();
		$pos = (new Query('SELECT `x`, `y`, `scene` FROM `PlayerData` WHERE `uuid`=:uuid;', [':uuid' => $uuid]))->fetch();

        (new APIView([
			'pos' => $pos,		// Player position [x, y]
			'inv' => $inv		// Inventory [Item]
		]))->render();
	}
}
