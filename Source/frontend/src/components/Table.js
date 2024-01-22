import React from "react";
import TextConstant from "../constants/text";

const Table = ({ data = [], columns = [], className, isLoading = false }) => {
  const hasData = data.length > 0;

  return (
    <table className={`border-collapse ${className}`}>
      <thead>
        <tr>
          {columns.map((column) => (
            <th
              key={column.key}
              className={`py-2 font-bold text-sm pl-4 h-[40px] text-center bg-gray text-gray ${column.className}`}
            >
              {column.label}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {hasData && !isLoading
          ? data.map((item, index) => (
              <tr key={index} className={"even:bg-gray-light h-[70px]"}>
                {columns.map((column) => (
                  <td
                    key={`${column.key}-${index}`}
                    className={` text-gray pl-6 text-md ${column.className}`}
                  >
                    {column.component ? (
                      column.component(item)
                    ) : (
                      <span>{item[column.key]}</span>
                    )}
                  </td>
                ))}
              </tr>
            ))
          : null}

        {!hasData && !isLoading ? (
          <tr>
            <td colSpan={columns.length} className="py-2 px-4 text-center">
              {TextConstant.empytText}
            </td>
          </tr>
        ) : null}
      </tbody>
    </table>
  );
};

export default Table;
