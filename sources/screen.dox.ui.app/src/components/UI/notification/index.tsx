import React from 'react';
import { useSnackbar } from 'notistack';
import { useDispatch, useSelector } from 'react-redux';
import { 
    isNotificationSelector, getNotificationMessageSelector, getNotificatioStatusSelector 
} from '../../../selectors/settings';
import { resetNotify } from '../../../actions/settings';

const CustomNotification = (): React.ReactElement => {
    
    let timer: NodeJS.Timeout | null = null; 
    const dispatch = useDispatch();
    const { enqueueSnackbar, closeSnackbar } = useSnackbar();
    const isNotification = useSelector(isNotificationSelector);
    const message = useSelector(getNotificationMessageSelector);
    const variant = useSelector(getNotificatioStatusSelector);

    React.useEffect(() => {
        if (isNotification && message) {
            if(timer) {
                clearTimeout(timer);
            }  
            enqueueSnackbar(message, { variant })
            timer = setTimeout(() => {
                dispatch(resetNotify())
                closeSnackbar();
            }, 3000);
        }
        return () => {
            if(timer) {
                clearTimeout(timer);
            }
        }
    }, [dispatch, isNotification, message]);

    return (<div id="notofication-tag"/>)
}

export default CustomNotification;