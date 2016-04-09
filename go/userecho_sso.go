package userecho

import (
    "crypto/aes"
    "crypto/cipher"
    "crypto/sha1"
    "encoding/base64"
    "encoding/json"
    "fmt"
    "net/url"
    "strings"
    "time"
)

var userEchoSsoKey string = "YOUR_KEY"
var userEchoProjectAlias string = "YOUR_ALIAS"

type UserEchoJSON struct {
    UserId      string `json:"guid"`
    ExpiresTime string `json:"expires_date"`
    DisplayName string `json:"display_name"`
    Email       string `json:"email"`
    Verified    bool   `json:"verified_email"`
}


func GetSSOToken(guid string, expiresTime time.Time, displayName, email string) (string, []error) {

    expiresTimeString := fmt.Sprintf("%d-%02d-%02d %02d:%02d:%02d",
        expiresTime.Year(),
        expiresTime.Month(),
        expiresTime.Day(),
        expiresTime.Hour(),
        expiresTime.Minute(),
        expiresTime.Second())

    // Create Json string
    userEchoJson := &UserEchoJSON{
        UserId:      guid,
        ExpiresTime: expiresTimeString,
        DisplayName: displayName,
        Email:       email,
        Verified:    true,
    }

    jsonBytes, e := json.Marshal(userEchoJson)
    if e != nil {
        return "", e
    }

    // Generate IV
    ivString := handyman.RandomAlphanumericString(aes.BlockSize)
    iv := []byte(ivString)


    // Make a hash key
    hasher := sha1.New()
    hasher.Write([]byte(userEchoSsoKey + userEchoProjectAlias))
    hashKey := hasher.Sum(nil)[:16]


    // XOR the first 16 characters of JSON object with the IV 
    // (so we include the vector in the data)
    for i := 0; i < aes.BlockSize; i++ {
        jsonBytes[i] = jsonBytes[i] ^ iv[i]
    }

    // Expand block
    pad := aes.BlockSize - (len(jsonBytes) % aes.BlockSize)
    data := []byte(string(jsonBytes[:]) + strings.Repeat(fmt.Sprintf("%c", rune(pad)), pad))

    // Ecrypt the data
    var block cipher.Block
    block, e = aes.NewCipher(hashKey)
    if e != nil {
        return "", e
    }
    mode := cipher.NewCBCEncrypter(block, iv)
    mode.CryptBlocks(data, data)
    data64 := base64.StdEncoding.EncodeToString(data)

    // Screen out characters not allowed in links
    result := url.QueryEscape(data64)

    return result, e.Nothing()
}