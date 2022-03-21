import axios from  '../axios';

const getTypeheadCountyOfResidence = async (query: string): Promise<Array<string>> => {

    return await axios.instance.get(`typeahead/county?q=${query}`);
}

export default getTypeheadCountyOfResidence;