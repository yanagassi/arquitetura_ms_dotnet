import React from "react";

const DateFilterButtons = ({ handleAutoFill }) => {
  return (
    <div className="cursor-default">
      <span className="text-primary"> Ultimos:</span>
      <button
        onClick={() => handleAutoFill(1)}
        className="py-2 px-4 -mr-4 -mt-2 cursor-pointer bg-gray-300 text-primary rounded-md hover:bg-gray-400 focus:outline-none focus:ring focus:border-gray-200"
      >
        1 dia
      </button>
      <button
        onClick={() => handleAutoFill(7)}
        className="py-2 px-4 -mr-4 -mt-2 cursor-pointer bg-gray-300 text-primary rounded-md hover:bg-gray-400 focus:outline-none focus:ring focus:border-gray-200"
      >
        7 dias
      </button>
      <button
        onClick={() => handleAutoFill(15)}
        className="py-2 px-4 -mr-4  -mt-2 cursor-pointer bg-gray-300 text-primary rounded-md hover:bg-gray-400 focus:outline-none focus:ring focus:border-gray-200"
      >
        15 dias
      </button>
      <button
        onClick={() => handleAutoFill(30)}
        className="py-2 px-4  -mt-2 bg-gray-300 cursor-pointer text-primary rounded-md hover:bg-gray-400 focus:outline-none focus:ring focus:border-gray-200"
      >
        30 dias
      </button>
    </div>
  );
};

export default DateFilterButtons;
