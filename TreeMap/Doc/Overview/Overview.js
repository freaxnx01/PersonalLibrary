
//
// Overview.js
//
// Contains functions shared by the Overview.htm files for each of the
// visualization component overview pages, as well as the overview page that
// describes all of the components.
//
// In VSS, there is a single Overview.js file that is shared among all the
// Overview directories.  On the drop server, however, each Overview directory
// has its own copy.  It would be possible to have all the Overview.htm files
// reference a shared Overview.js file in a central drop directory, but that
// kind of dependency would prevent a customer from simply copying an Overview
// directory to his machine.
//

//
// Opens the Overview page for the visualization components.
//
function OpenVisualizationComponentOverview()
{
	window.navigate("..\\..\\..\\..\\Documents\\Overview\\Overview.htm");
}

//
// Opens the "Latest" directory for one of the visualization components.
// sComponent is "BubbleChart", for example.
//
function OpenComponentLatestDirectory(sComponent)
{
	// (sComponent is ignored in the public release.)

	window.navigate("..\\..");
}
