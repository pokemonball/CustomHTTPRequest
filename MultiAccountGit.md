## 🦄Handling multi account for github 🧑🏻🧓🏻
---
#### 🆕 Create git local config 
To enable multi account for github enable the local config for user.name and user.email 
```sh 
git config user.name 'pokemonball'
git config user.email 'youremail@gmail.com'
```
Check the config using this command 
```sh 
git config --get user.name
git config --get user.email
```
#### 🔗 Check the remote information 
Check git remote information 
```sh 
git remote show origin

  Fetch URL: https://pokemonball@github.com/pokemonball/CustomHTTPRequest.git
  Push  URL: https://pokemonball@github.com/pokemonball/CustomHTTPRequest.git
  HEAD branch: main
  Remote branches:
    main               tracked
    pix3lize/issue1    tracked
    pokemonball/issue2 tracked
  Local branches configured for 'git pull':
    main               merges with remote main
    pix3lize/issue1    merges with remote pix3lize/issue1
    pokemonball/issue2 merges with remote pokemonball/issue2
  Local refs configured for 'git push':
    main               pushes to main               (up to date)
    pix3lize/issue1    pushes to pix3lize/issue1    (up to date)
    pokemonball/issue2 pushes to pokemonball/issue2 (up to date)
```
#### 🚥Modify remote information 
Delete the existing remote 
```sh
git remote rm origin
```
Re-add the remote information 
```sh
git remote add origin https://pokemonball@github.com/pokemonball/CustomHTTPRequest.git
```
#### Removing the account in mac
```sh
 $ git credential-osxkeychain erase ⏎
 host=github.com  ⏎
 protocol=https   ⏎
 ⏎
 ⏎   
```