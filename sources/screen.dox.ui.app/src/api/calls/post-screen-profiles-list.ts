import axios from  '../axios';
import { IScreenProfilesResponseItem, IScreenProfilesRequestItem } from '../../actions/screen-profiles';

const postScreenProfilesList = async (props: IScreenProfilesRequestItem): Promise<{
    Items: Array<IScreenProfilesResponseItem>;
    TotalCount: number;
}> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`screeningprofile/search`, {
        ...replace,
      });
}

export default postScreenProfilesList;