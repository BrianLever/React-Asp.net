import React from 'react';
import styled from 'styled-components';
import ResponsiveDrawer from '../side-bar';
import { withRouter } from 'react-router-dom';
import { IPage } from '../../../common/types/page';

const Wraper = styled.div`
    display: flex;
`;

export interface ILayoutProps extends IPage {
    children: any;
}

export interface ILayoutState {}

class Layout extends React.Component<Readonly<ILayoutProps>, ILayoutState> {

    public render():React.ReactElement {
        return (
            <Wraper>
                <ResponsiveDrawer window={() => window} {...this.props}/>
            </Wraper>
        )
    }

}

const LayoutCopy = withRouter(Layout);

export default LayoutCopy;