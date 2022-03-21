
export interface INotMatchesFieldsCheckProps {
    notMatchFields: Array<string> | undefined;
    field: string;
}
const IsNotMatchesFieldCheck = (props: INotMatchesFieldsCheckProps) => {
    const matcheFields = props.notMatchFields && props.notMatchFields.filter(field => field === props.field);
    if(matcheFields && matcheFields.length !== 0) {
        return true;
    }
    return false;
}

export default IsNotMatchesFieldCheck;