import React from "react";
import { render, fireEvent } from "@testing-library/react";
import DateFilterButtons from "../components/DateFilterButtons";

describe("DateFilterButtons", () => {
  it("deve chamar handleAutoFill com o valor correto quando um botão é clicado", () => {
    const handleAutoFillMock = jest.fn();
    const { getByText } = render(
      <DateFilterButtons handleAutoFill={handleAutoFillMock} />
    );

    fireEvent.click(getByText("1 dia"));
    expect(handleAutoFillMock).toHaveBeenCalledWith(1);

    fireEvent.click(getByText("7 dias"));
    expect(handleAutoFillMock).toHaveBeenCalledWith(7);

    fireEvent.click(getByText("15 dias"));
    expect(handleAutoFillMock).toHaveBeenCalledWith(15);

    fireEvent.click(getByText("30 dias"));
    expect(handleAutoFillMock).toHaveBeenCalledWith(30);
  });

  it("deve ter o estilo correto para os botões", () => {
    const { getByText } = render(
      <DateFilterButtons handleAutoFill={() => {}} />
    );

    const buttons = [
      getByText("1 dia"),
      getByText("7 dias"),
      getByText("15 dias"),
      getByText("30 dias"),
    ];

    buttons.forEach((button) => {
      expect(button).toHaveClass(
        "bg-gray-300 text-primary rounded-md hover:bg-gray-400"
      );
    });
  });
});
