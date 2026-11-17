function New-SecurePassword {
    [CmdletBinding()]
    param(
        [ValidateRange(1, 1024)]
        [int]$Length = 24
    )

    $chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*-_?'
    $password = [char[]]::new($Length)

    for ($i = 0; $i -lt $Length; $i++) {
        $index = [System.Security.Cryptography.RandomNumberGenerator]::GetInt32($chars.Length)
        $password[$i] = $chars[$index]
    }

    -join $password
}

Export-ModuleMember -Function New-SecurePassword
