function getCurrentDate() {
  const currentDate = new Date();
  const year = currentDate.getUTCFullYear();
  const month = String(currentDate.getUTCMonth() + 1).padStart(2, "0");
  const day = String(currentDate.getUTCDate()).padStart(2, "0");

  const formattedDate = `${year}-${month}-${day}`;
  return formattedDate;
}
function formatDateTimeColumn(dateTimeString) {
  const options = {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  };

  const date = new Date(dateTimeString);
  const formattedDateTime = date
    .toLocaleString("en-US", options)
    .replace(/(\d+)\D+(\d+)\D+(\d+)\D+(\d+)\D+(\d+)/, "$1.$2.$3 $4:$5");

  return formattedDateTime.replace("AM", "").replace("PM", "");
}

function formatMoney(numero, locale = "pt-BR", currency = "BRL") {
  return numero.toLocaleString(locale, {
    style: "currency",
    currency,
  });
}

function getTypeCreditOrDebit(tipo) {
  if (typeof tipo === "string") tipo = parseInt(tipo);
  switch (tipo) {
    case 0:
      return "Crédito";
    case 1:
      return "Debito";
  }
}

export default {
  getCurrentDate,
  formatDateTimeColumn,
  formatMoney,
  getTypeCreditOrDebit,
};
