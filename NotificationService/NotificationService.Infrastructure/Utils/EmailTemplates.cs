namespace NotificationService.Infrastructure.Utils;

public static class EmailTemplates
{
    public static string Invite(string organizationName)
    {
        // TODO добавить нужную ссылку
        return $@"
            <h2>Вас пригласили в организацию: {organizationName}</h2>
            <p>Нажмите на ссылку, чтобы принять приглашение:</p>
            <p><a href='Test'>Присоедениться</a></p>
            <br>
            <p>Если вы не ожидаете приглашения, вы можете проигнорировать это сообщение.</p>";
    }
}