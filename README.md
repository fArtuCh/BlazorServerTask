# Blazor ShowCase Project 

This project is written in .NET 7.0, utilizing Blazor server.

## Known Issues

- When dragging multiple elements (by holding the shift key), a user card can only be grabbed by the picture.
- It is not possible to drag users directly between groups (for example, from group 1 to group 2). This is because when a tab of another group is opened, the currently held card ceases to exist. To solve this issue, the application needs to be refractor. All groups should exist simultaneously, but only the active group should be visible and handle the application's logic.

## Usage

Drag and drop cards of people to change their group. Right click to lock them in a given group. Single click to move given card at the start of the group. Hold shift to move multiple cards (hold by user picture).


## Additional Libraries/Packages Used

- Mediator (only for defining interfaces).
- Bootstrap 5 (for application styling)

## Project Structure

1. **LocalServices**: This contains the application logic and stored data. For testing purposes, data is stored in local memory. Blazor components access the service through a prepared manager with interfaces.

2. **PagesLib**: This contains only the application's pages. For larger projects, this is a convenient solution.

3. **ComponentsLib**: Application components.

4. **Domain**: Models, Constants, Enumerations, and similar. All projects can access it.

5. **BlazorExample**: The Startup project for the Blazor Server application.

To run the application, execute the BlazorExample project.