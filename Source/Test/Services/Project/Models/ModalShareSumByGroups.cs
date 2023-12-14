namespace Tests.Services.Models;

public record ModalShareSumByGroups(
	decimal ExpedicaoRodoviario,
	decimal ExpedicaoFerroviario,
	decimal ExpedicaoHidroviario,
	decimal ExpedicaoMaritimo,
	
	decimal RececaoRodoviario,
	decimal RececaoFerroviario,
	decimal RececaoHidroviario,
	decimal RececaoMaritimo,
	bool hasError = false
	);
