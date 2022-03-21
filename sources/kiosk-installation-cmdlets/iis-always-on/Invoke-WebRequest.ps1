param(
    [string]$Url
)


$r = [System.Net.WebRequest]::Create("$Url")
$resp = $r.GetResponse()
$reqstream = $resp.GetResponseStream()
$sr = new-object System.IO.StreamReader $reqstream
$result = $sr.ReadToEnd()
write-host $result