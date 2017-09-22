using System.Linq;
using System.Text;

public static class Helper
{
    public static string GetFriendlyName(this string input)
    {
        var sb = new StringBuilder();
        foreach (var c in input)
        {
            if (char.IsUpper(c))
                sb.Append(" ");
            sb.Append(c);
        }

        if (input.Length > 0 && char.IsUpper(input[0]))
            sb.Remove(0, 1);

        return sb.ToString();
    }

    public static string GenerateRoomName(string roomName)
    {
        if (PhotonNetwork.offlineMode)
            return "Solo";
        var rooms = PhotonNetwork.GetRoomList();
        var roomNames = rooms.Select(room => room.Name).ToList();
        int i = 0;
        var internalName = roomName + i;

        if (roomNames.Contains(internalName))
        {
            i++;
            internalName = roomName + i;
        }
        return internalName;
    }
}