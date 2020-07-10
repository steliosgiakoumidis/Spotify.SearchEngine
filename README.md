# Spotify.SearchEngine

## General Info

This project is a spotify search engine for artists. Passing the name of the artists you receive a list of all matching artists name together with a spotify url of their tracks and albums. 

## Api Documentation
For a detailed API Documentation please run the application and browse to the swagger page ( https://localhost/swagger)

## How to run
The solution contains docker support. In order to build the image browse with powershell to the Dockerfile folder and type 

```
docker build . -t spotify_search
```
After the image is created run the container by typing

```
docker run -e AuthenticationUrl=https://accounts.spotify.com/api/token -e SearchUrl=https://api.spotify.com/v1/search -e ClientId={client id} -e ClientSecret={client secret} -p 80:80 spotify_search
```
The application settings are passed as environment variables (and not as environment dependent appsettings.json files) so they are not compromised by getting exposed through the source control.

## Support
In case questions contact me at stelios.giakoumidis[at]gmail.com