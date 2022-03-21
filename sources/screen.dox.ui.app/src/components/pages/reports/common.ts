export const formatPercentageValue = (x: any) : string =>
{
    let result = x;

    if(x === "NaN"){
        result = "--";
    }
    return result + "%";
}