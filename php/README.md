Change `UserEcho::API_KEY`, `UserEcho::PROJECT_KEY`, `UserEcho::BASE_URL` adn use it.

    $ue = new UserEcho();
    $url = $ue->createSsoUrl('id', 'login', 'email', 'avatar');
    // profit
