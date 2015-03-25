/**
 * Program.cs
 * 
 * Written by Jonathan Darling - 17 February 2015.
 * 
 * Developed in Xamarin Studio - OS X 10.10.
 * 
 * Tested working on Windows 7 x64.
 * 
 * Program used to calculate the total "name score" given a file with comma seperated names.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace nameScoreCalculator
{
	class MainClass
	{

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args)
		{
			int totalScore;
			string fileName;
			List<string> names;

			// Get the name of the file containing the comma seperated names.
			if (args.Length > 0) {
				fileName = args [0];
			} else {
				fileName = getFileNameFromUser ();
			}

			// If the user has not yet entered a correct file name, continue asking the user for a file name until they
			// enter a correct one.
			while (!System.IO.File.Exists (fileName)) {
				Console.WriteLine ("The file you specified was not found.");
				fileName = getFileNameFromUser ();
			}

			try {
				names = getNamesFromFile (fileName);
				names.Sort ();
				totalScore = calculateTotalScore (names);
				Console.WriteLine ("The total score for the input file is " + totalScore + ".");

				// Keep window open until user exits.
				Console.Write ("Press enter to end...");
				Console.ReadLine ();

			} catch (FileNotFoundException ex) {
				Console.WriteLine ("The file you specified was not found, please try again.");
				Console.WriteLine (ex.StackTrace);
			}
		}

		/// <summary>
		/// Gets the file name from user.
		/// </summary>
		/// <returns>The user entered file name.</returns>
		private static string getFileNameFromUser ()
		{
			Console.Write ("\nPlease enter a file name: ");
			return Console.ReadLine ();
		}

		/// <summary>
		/// Get the names in the given file.
		/// </summary>
		/// <param name="fileName">The name of the file to get names from.</param>
		/// <returns>The names from the file.</returns>
		private static List<string> getNamesFromFile (string fileName)
		{
			string fileContents;
			string[] namesArray;

			// Ensure that the file we are trying to open exists.
			if (System.IO.File.Exists (fileName)) {
				System.IO.StreamReader streamReader = new System.IO.StreamReader (fileName);
				fileContents = streamReader.ReadToEnd ();

				// Remove parenthesis.
				fileContents = fileContents.Replace ("\"", "");

				// Split string at commas.
				namesArray = fileContents.Split (new Char[] { ',' });

				// Convert the array of names to a list and return.
				return new List<string> (namesArray);

			} else {
				throw new FileNotFoundException ("The file " + fileName + " was not found.");
			}
		}

		/// <summary>
		/// Calculates the total score for the list of names.
		/// </summary>
		/// <param name="names">List of names to calculate the score for.</param>
		/// <returns>The total score for the list of names.</returns>
		private static int calculateTotalScore (List<string> names)
		{
			int placeInList;
			int score = 0;

			for (int i = 0; i < names.Count; i++) {
				placeInList = (i + 1);
				score += (placeInList * calculateNameScore (names [i]));

				// DEBUG ONLY
				// Console.WriteLine ("[DEBUG] " + placeInList + " : " + names [i] + " : " + (placeInList * calculateNameScore (names [i])));
			}

			return score;
		}

		/// <summary>
		/// Calculates the score for an individual name.
		/// </summary>
		/// <param name="name">The name to calculate the score for.</param>
		/// <returns>the score for the individual name.</returns>
		private static int calculateNameScore (string name)
		{
			int score = 0;

			foreach (char character in name) {

				// Adjust to find character position in alphabet.
				score += ((int)char.ToUpper (character) - 64);
			}
			return score;
		}
	}
}
