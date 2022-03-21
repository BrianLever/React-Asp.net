import React from 'react';
import Drawer from '@material-ui/core/Drawer';
import { useDispatch, useSelector } from 'react-redux';
import { getSideDrawerState } from '../../../selectors/settings';
import { triggerSideDrawerState } from '../../../actions/settings';


type Anchor = 'top' | 'left' | 'bottom' | 'right';

export type TScreendoxDrawerProps = {
    anchor: Anchor;
    children: any;
}

export default function ScreendoxDrawer(props: TScreendoxDrawerProps): React.ReactElement {

    const dispatch = useDispatch();
    const isOpen = useSelector(getSideDrawerState);

    return (
        <Drawer 
            anchor={props.anchor} 
            open={isOpen} 
            onClose={(event: any) => {
                if (
                    event.type === 'keydown' &&
                    ((event as React.KeyboardEvent).key === 'Tab' ||
                      (event as React.KeyboardEvent).key === 'Shift')
                ) {
                    return;
                } else {
                    dispatch(triggerSideDrawerState(false));
                }
            }}
        >
            <div 
                style={{ width: '300px' }}
            >
                { props.children }
            </div>
        </Drawer>
    );
}