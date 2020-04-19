// MyClass
using ConsoleApp1;
using EntityMappingDBCore.DBToEntity;
using System;
using System.Data;

public Person MyMethod(DataRow P_0)
{
	//IL_0060: Expected O, but got F8
	Person person = new Person();
	if (DynamicAssembleInfo.CanSetted(P_0, "Id"))
	{
		person.set_Id(Convert.ToInt32(P_0["Id"]));
	}
	if (DynamicAssembleInfo.CanSetted(P_0, "DTO"))
	{
		object obj = Convert.ToString(P_0["DTO"]);
		if (DynamicAssembleInfo.ScientificNotation((string)obj))
		{
			obj = Convert.ToDouble(obj);
		}
		person.set_DTO(Convert.ToDecimal(obj));
	}
	return person;
}
