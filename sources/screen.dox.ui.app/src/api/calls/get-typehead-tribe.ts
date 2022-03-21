import axios from  '../axios';

const getTypeheadTribe = async (query: string): Promise<Array<string>> => {

    return await axios.instance.get(`typeahead/tribe?q=${query}`);
}

export default getTypeheadTribe;