/*
    "Go" UserEcho Single Sign-On code example v2.0
    Date: 2016-05-04
    
    Using: 
    Add your userEchoSsoKey. call GetSSOToken()
    Then use sso_token in your URL: http://[your_alias].userecho.com/?sso_token=sso_token
    OR in the JS widget:
    var _ues = {
        ... ,
    params:{sso_token:sso_token}
    };
*/
package main

import (
    "crypto/aes"
    "crypto/cipher"
    "encoding/base64"
    "encoding/json"
    "fmt"
    "net/url"
    "strings"
    "time"
)

var userEchoSsoKey string = "==========YOUR_SSO_KEY=========="

type UserEchoJSON struct {
    UserId      string `json:"guid"`
    Expires     int64  `json:"expires"`
    DisplayName string `json:"display_name"`
    Email       string `json:"email"`
}

func GetSSOToken(guid, displayName, email string) (string, error) {

    expiresTimeString := time.Now().Unix() + 3600

    // Create Json string
    userEchoJson := &UserEchoJSON{
        UserId:      guid,
        Expires: expiresTimeString,
        DisplayName: displayName,
        Email:       email,
    }

    jsonBytes, e := json.Marshal(userEchoJson)
    if e != nil {
        return "", e
    }

    // Generate IV
    ivString := handyman.RandomAlphanumericString(aes.BlockSize)
    
    iv := []byte(ivString)

    // Expand block
    pad := aes.BlockSize - (len(jsonBytes) % aes.BlockSize)
    data_string := string(jsonBytes[:]) + strings.Repeat(fmt.Sprintf("%c", rune(pad)), pad)
    data := []byte(data_string)
    
    // Ecrypt the data
    var block cipher.Block
    block, e = aes.NewCipher([]byte(userEchoSsoKey))
    if e != nil {
        return "", e
    }
    mode := cipher.NewCBCEncrypter(block, iv)
    mode.CryptBlocks(data, data)
    data64 := base64.StdEncoding.EncodeToString(append(iv, data...))

    // Screen out characters not allowed in links
    result := url.QueryEscape(data64)

    return result, nil
}


func main(){
    var x, e = GetSSOToken("xsxsx", "cdccd", "cdcdcd")
    if e != nil {
        fmt.Printf("%s\n", e)
    }
    fmt.Printf("%s\n", x)
}
