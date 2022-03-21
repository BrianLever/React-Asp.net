import axios from  '../axios';

const postEhrExportResult = async (screeningResultID: number, props: {
    PatientId: number,
    VisitId: number
}): Promise<{ "Status": number,
        "IsSuccessful": boolean,
        "OperationStatusText": string,
        "Errors": Array<string>,
        "ExportResults": Array<string>,
        "ExportScope": any}> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('ehrexport/result/'+screeningResultID, {     
      ...replace
   });
}

export default postEhrExportResult;
