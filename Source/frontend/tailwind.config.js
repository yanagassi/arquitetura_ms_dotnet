/** @type {import('tailwindcss').Config} */

module.exports = {
  content: ["./src/**/*.{html,js}"],
  theme: {
    extend: {
      colors: {
        primary: "#025FEB",
        secondary: "#182C62",
        white: "#FFFFFF",
        gray: "#4B5C68",
      },
      backgroundColor: {
        primary: "#025FEB",
        secondary: "#182C62",
        white: "#FFFFFF",
        gray: "#E4EDF2",
        "gray-light": "#F6F7F7",
      },
      height: {
        16: 60,
      },
      margin: {
        16: 60,
        5: 20,
      },
      lineHeight: {
        lg: 0,
        md: 0,
        sm: 0,
      },
      fontSize: {
        lg: 24,
        lmd: 16,
        md: 14,
        sm: 12,
      },
      padding: {
        lg: 40,
        md: 20,
      },
      width: {
        12: 53,
      },
      height: {
        8: 37,
      },
    },
  },
  plugins: [],
};
