### Monogame Engine Changelog

##### 14/09/2019 - dunkybaldy
* Add engine diagnostics, can wrap targeted functions in a diagnose function from the controller
* Initial work to add event management, starting with keyboard (TODO)
* Support for creating 3d entities
* Initial work to allow for game settings to be added and managed through the engine making it generic for every game (TODO)
* Ability to draw using matrices defined in the entity class
* Added NUnit test project

##### 24/08/2019 - dunkybaldy
* Set up event handling to run on a separate thread.
  * Task.Factory.StartNew(action)
* Diagnose high level functions
* Allow game title easy change
  * OnActivated and OnDeactivated methods can be overidden
* Simple start up from game example main method

##### 19/08/2019 - dunkybaldy
* Set up barebones event system
  * Nothing consumes it at the moment
* Focus creating on 3D engine
  * 2D functionality is desired in the future though

##### 18/08/2019 - dunkybaldy
* AssetManager added
* Easy loading of models, textures, sounds etc
* Monogame Content folder added with connected mgcb process
* Added basic content to content folder

##### 28/07/2019 - dunkybaldy
* Set up base engine with entity management
* Example test suite
* Async Await functionality
* Dependency Injection functionality