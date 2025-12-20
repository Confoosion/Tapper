using System;
using System.Threading.Tasks;
using UnityEngine;
using Apple.GameKit;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class LoginManager : MonoBehaviour
{
    string Signature;
    string TeamPlayerID;
    string Salt;
    string PublicKeyUrl;
    ulong Timestamp;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        #if UNITY_EDITOR
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Signed in anonymously (editor mode)");

        #elif UNITY_IOS
        await AuthenticateViaGameCenter();

        #endif
    }

    public async Task AuthenticateViaGameCenter()
    {
        if (!GKLocalPlayer.Local.IsAuthenticated)
        {
            // Perform the authentication.
            var player = await GKLocalPlayer.Authenticate();
            Debug.Log($"GameKit Authentication: player {player}");

            // Grab the display name.
            var localPlayer = GKLocalPlayer.Local;
            Debug.Log($"Local Player: {localPlayer.DisplayName}");

            // Fetch the items.
            var fetchItemsResponse = await GKLocalPlayer.Local.FetchItems();

            Signature = Convert.ToBase64String(fetchItemsResponse.GetSignature());
            TeamPlayerID = localPlayer.TeamPlayerId;
            Debug.Log($"Team Player ID: {TeamPlayerID}");

            Salt = Convert.ToBase64String(fetchItemsResponse.GetSalt());
            PublicKeyUrl = fetchItemsResponse.PublicKeyUrl;
            Timestamp = fetchItemsResponse.Timestamp;

            Debug.Log($"GameKit Authentication: signature => {Signature}");
            Debug.Log($"GameKit Authentication: publickeyurl => {PublicKeyUrl}");
            Debug.Log($"GameKit Authentication: salt => {Salt}");
            Debug.Log($"GameKit Authentication: Timestamp => {Timestamp}");
        }
        else
        {
            Debug.Log("AppleGameCenter player already logged in.");
        }
    }

    public void StartSignInOrLinkWithAppleGameCenter()
    {
        // UnityServices.InitializeAsync(); // IDK WTF THIS DOES
        SignInOrLinkWithAppleGameCenter();
    }

    private async void SignInOrLinkWithAppleGameCenter()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await SignInWithAppleGameCenterAsync(Signature, TeamPlayerID, PublicKeyUrl, Salt, Timestamp);
        }
        else
        {
            await LinkWithAppleGameCenterAsync(Signature, TeamPlayerID, PublicKeyUrl, Salt, Timestamp);
        }
    }

    async Task SignInWithAppleGameCenterAsync(string signature, string teamPlayerId, string publicKeyURL, string salt, ulong timestamp)
    {
        #if UNITY_IOS && !UNITY_EDITOR
        try
        {
            await AuthenticationService.Instance.SignInWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        
        #else
        Debug.LogWarning("Game Center authentication only works on iOS devices");
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        #endif
    }
    
    async Task LinkWithAppleGameCenterAsync(string signature, string teamPlayerId, string   publicKeyURL, string salt, ulong timestamp)
    {
        #if UNITY_IOS && !UNITY_EDITOR
        try
        {
            await AuthenticationService.Instance.LinkWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            // Prompt the player with an error message.
            Debug.LogError("This user is already linked with another account. Log in instead.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        
        #else
        Debug.LogWarning("Game Center linking only works on iOS devices. Skipping in Editor.");
        // Don't attempt to link in Editor - just return
        await Task.CompletedTask;
        #endif
    }
}
