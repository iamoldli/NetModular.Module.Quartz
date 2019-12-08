<template>
  <nm-dialog class="nm-quartz-job-add" v-bind="dialog" @open="onOpen" :visible.sync="visible_">
    <nm-tabs :fullscreen="false">
      <el-tabs v-model="tab" type="border-card">
        <!--通用任务-->
        <el-tab-pane name="normal">
          <span slot="label"> <nm-icon name="basic-data"></nm-icon>通用任务 </span>
          <normal-page ref="normal" @success="onSuccess" />
        </el-tab-pane>
        <!--Http任务-->
        <el-tab-pane name="http">
          <span slot="label"> <nm-icon name="web"></nm-icon>HTTP任务 </span>
          <http-page ref="http" @success="onSuccess" />
        </el-tab-pane>
      </el-tabs>
    </nm-tabs>
  </nm-dialog>
</template>
<script>
import { mixins } from 'netmodular-ui'
import NormalPage from './normal'
import HttpPage from './http'

export default {
  mixins: [mixins.dialog],
  components: { NormalPage, HttpPage },
  data() {
    return {
      tab: 'normal',
      dialog: {
        title: '添加任务',
        icon: 'add',
        width: '900px',
        height: '600px',
        fullscreen: true
      }
    }
  },
  methods: {
    onOpen() {
      this.$nextTick(() => {
        this.$refs.normal.reset()
        this.$refs.http.reset()
      })
    },
    onSuccess() {
      this.hide()
      this.$emit('success')
    }
  }
}
</script>
<style lang="scss">
.nm-quartz-job-add {
  .el-scrollbar__view {
    padding: 0 !important;
    height: 100%;
  }
}
</style>
