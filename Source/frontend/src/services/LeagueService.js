import ConfigConstants from "../constants/config";

import api from "./api";

/**
 * A class representing a service that processes the data for match schedule
 * and generates leaderboard.
 */
class LeagueService {
  constructor() {
    this.matches = [];
    this.token = null;

    this.apiVersion = null;
  }

  /**
   * Sets the matches data for the service.
   * @param {Array} matches - An array of match objects.
   */
  setMatches(matches) {
    this.matches = matches;
  }

  /**
   * Retrieves and formats match data, including team images, match date, and score display.
   * @returns {Array} An array of formatted match objects.
   */
  getMatches() {
    const { imageApi, imageDefaultExtension } = ConfigConstants;

    return this.matches.map((match) => ({
      ...match,
      imageHomeTeam: `${imageApi}${match.homeTeam}${imageDefaultExtension}`,
      imageAwayTeam: `${imageApi}${match.awayTeam}${imageDefaultExtension}`,

      scoreDisplay: match.matchPlayed
        ? `${match.homeTeamScore} : ${match.awayTeamScore}`
        : "- : -",
    }));
  }

  /**
   * Retrieves the current leaderboard based on match data.
   * @returns {Array} An array representing the current leaderboard.
   */
  getLeaderboard() {
    const leaderboard = this.calculateLeaderboard();
    return leaderboard;
  }

  /**
   * Asynchronously fetches match data by authenticating and updating the matches property.
   * Only calls authentication if the token is not found.
   * @throws {Error} If there is an issue fetching or updating match data.
   */
  async fetchData() {
    try {
      if (!this.token) await this.authenticate();

      const matches = await this.fetchMatches();
      this.setMatches(matches);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  }

  /**
   * Asynchronously authenticates and sets the access token for the service.
   * Set the token to localstorage so there is no need for future authentication.
   * @throws {Error} If there is an issue with the authentication process.
   */

  async authenticate() {
    try {
      const { data } = await api.get("/api/v1/getAccessToken");
      localStorage.setItem(ConfigConstants.tokenEnum, data.access_token);
      this.token = data.access_token;
    } catch (error) {
      console.error("Authentication error:", error);
      throw error;
    }
  }

  /**
   * Checks and sets the authentication token from localStorage.
   * @returns {boolean} Returns true if a token is present in localStorage and sets it in the service;
   * otherwise, returns false.
   */
  checkAndSetAuthToken() {
    const token = localStorage.getItem(ConfigConstants.tokenEnum);

    if (token) {
      this.token = token;
      return true;
    }

    return false;
  }

  /**
   * Asynchronously fetches match data using the stored access token.
   * @returns {Array} An array of match objects.
   * @throws {Error} If there is an issue fetching match data.
   */
  async fetchMatches() {
    try {
      const { data } = await api.get("/api/v1/getAllMatches");
      return data.matches;
    } catch (error) {
      console.error("Error fetching matches:", error);
      throw error;
    }
  }

  /**
   * Calculates and generates a leaderboard based on match data.
   *
   * @returns {Array} An array representing the leaderboard.
   */
  calculateLeaderboard() {
    const teams = {};
    const { imageApi, imageDefaultExtension } = ConfigConstants;

    this.matches.forEach((match) => {
      if (!teams[match.homeTeam]) {
        teams[match.homeTeam] = {
          teamName: match.homeTeam,
          teamImage: `${imageApi}${match.homeTeam}${imageDefaultExtension}`,
          matchesPlayed: 0,
          goalsFor: 0,
          goalsAgainst: 0,
          points: 0,
        };
      }

      teams[match.homeTeam].matchesPlayed += 1;
      teams[match.homeTeam].goalsFor += match.homeTeamScore;
      teams[match.homeTeam].goalsAgainst += match.awayTeamScore;

      if (match.homeTeamScore > match.awayTeamScore) {
        teams[match.homeTeam].points += 3;
      } else if (match.homeTeamScore === match.awayTeamScore) {
        teams[match.homeTeam].points += 1;
      }

      if (!teams[match.awayTeam]) {
        teams[match.awayTeam] = {
          teamName: match.awayTeam,
          teamImage: `${imageApi}${match.awayTeam}${imageDefaultExtension}`,
          matchesPlayed: 0,
          goalsFor: 0,
          goalsAgainst: 0,
          points: 0,
        };
      }

      teams[match.awayTeam].matchesPlayed += 1;
      teams[match.awayTeam].goalsFor += match.awayTeamScore;
      teams[match.awayTeam].goalsAgainst += match.homeTeamScore;

      if (match.awayTeamScore > match.homeTeamScore) {
        teams[match.awayTeam].points += 3;
      } else if (match.awayTeamScore === match.homeTeamScore) {
        teams[match.awayTeam].points += 1;
      }
    });

    const leaderboard = Object.values(teams);
    leaderboard.sort((a, b) => b.points - a.points);

    return leaderboard;
  }

  async fetchApiVersion() {
    try {
      const { data } = await api.get("/api/version");
      this.apiVersion = data.version;
    } catch (error) {
      console.error("Authentication error:", error);
      return null;
    }
  }

  getApiVersion() {
    return this.apiVersion;
  }
}

export default LeagueService;
