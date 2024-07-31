### Work repartition
- DB: Azure, Models + Model testing
- DAO: DAO layer + DAO testing
- Service: Service layer + service testing
- Controller: Controller layer + auth
- Frontend: JS/HTML/CSS


## Pokemon collector

### Meeting requirements:
- **Application must build and run**
- **xUnit Testing: 70% Service + Model**
- **Front end unit testing**
- **Hosted on Github**
- **Utilize an external API**
	- Pokemon API
- **. Allows for multiple users to authenticate and store their data .**
	- Simple check on username + password combination
- **HTML/CSS/JS or React front-end**

### Idea
User-based webpage that lets you log in and add pokemons to your account, check your pokemons.

**User stories**
- As a user, I can log in and then click on the roll button to add some random pokemons into my account.
- As a user, I can log int and check out all the pokemons I have obtained so far.

**MVC**
- Controller
	- RESTful API for managing user and pokemons
- Service
	- Implements logic related to the things we can do (addPokemonsToTrainer, createTrainer, getAllPokemonsForTrainer, ...)
- DAO
	- CRUD operations for trainer, pokemon, collection
	
**Database tables**
|Trainer  |Pokemon |Collection |
|---------|--------|-----------|
|name     |id      |pokemon_id |
|password |        |trainer_id |
|id       |        |           |

**Front-end**
- Allow the user to send requests to register or log in
- Allow the user to send a request to add X pokemons to the account
- Allow the user to ask for all collected pokemons for that account. Display them

### Notes
- So far it seems that we dont need to hook up the server to the front end, meaning we can just "live server" the front end and have it use the backend API, we dont need the backend to return html.
- We can have the API interaction happen on the front end if we want.

### Going beyond requirements
- Improve authentication: Use JWT or other kind of authentication solution for authenticating.
- Have the backend reutrn the HTML too, so we dont have to use live-server.
- Make it so that you can only roll pokemons X times per day (3?)
- Make it so that when we check our collection, we can check the ones we have and the ones we lack (gray out the ones missing or something similar)
- Leaderboard that lists users ordered by who is closest to collecting them all.
- Add currency: If you get a duplicate in the roll, then just get currency instead. You can use that currency to roll again.
- Add some rules to how the RNG works:
	- Divide pokemons into rarity classes: normal, rare, S, SS
	- Divide pokemons into class: Fire, Water, ...
	- The more unique fire pokemons we have, the higher the chances of rolling a rare+ fire pokemon
- Add a shop, maybe we can sell pokemons to other users who pay with the webs currency... Allow for an acount to have up to x2 of the same pokemon.
