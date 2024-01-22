import React from "react";
import TextConstant from "../constants/text";
import { useApi } from "../context/ApiContext";

function Footer() {
  const { leagueService } = useApi();

  return (
    <footer className="bg-gray-light text-gray pr-lg text-right py-4 mt-8 bottom-0 w-full font-semibold">
      <span>{TextConstant.copyright}</span>
    </footer>
  );
}

export default Footer;
